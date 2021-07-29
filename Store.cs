using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Controllers
{
	internal interface IRedisClient
	{
		int Get(string type);
		void Set(string type, int current);
	}
	
	public static class FiguresStorage
	{
		// корректно сконфигурированный и готовый к использованию клиент Редиса
		private static IRedisClient RedisClient { get; }
	
        /* Эти два метода нужно объединить в один потокобезопасный метод */ 
		public static bool CheckIfAvailable(string type, int count)
		{
			return RedisClient.Get(type) >= count;
		}

		public static void Reserve(string type, int count)
		{
            /* Потоконебезопасно */
            var current = RedisClient.Get(type);

			RedisClient.Set(type, current - count);
		}
	}

	public class Position
	{
		public string Type { get; set; }

		public float SideA { get; set; }
		public float SideB { get; set; }
		public float SideC { get; set; }

		public int Count { get; set; }
	}

	public class Cart
	{
		public List<Position> Positions { get; set; }
	}

	public class Order
	{
		public List<Figure> Positions { get; set; }

        /* метод GetTotal можно перенести в Triangle и Circle, чтобы они сами считали свои Total. */
        /*  То есть здесь метод будет выглядеть так: public decimal GetTotal() => Positions.Select(p => p.GetTotal()).Sum(); */
        public decimal GetTotal() =>
			Positions.Select(p => p switch
				{
					Triangle => (decimal) p.GetArea() * 1.2m,
					Circle => (decimal) p.GetArea() * 0.9m
				})
				.Sum();
	}

	public abstract class Figure
	{
		public float SideA { get; set; }
		public float SideB { get; set; }
		public float SideC { get; set; }

		public abstract void Validate();
		public abstract double GetArea();
	}

	public class Triangle : Figure
	{
		public override void Validate()
		{
			bool CheckTriangleInequality(float a, float b, float c) => a < b + c;
			if (CheckTriangleInequality(SideA, SideB, SideC)
			    && CheckTriangleInequality(SideB, SideA, SideC)
			    && CheckTriangleInequality(SideC, SideB, SideA)) 
				return;
            /* В тексте исключений нужно больше информации. Помогает при дебаге */
			throw new InvalidOperationException("Triangle restrictions not met");
		}

		public override double GetArea()
		{
			var p = (SideA + SideB + SideC) / 2;
			return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
		}
		
	}
	
	public class Square : Figure
	{
		public override void Validate()
		{
			if (SideA < 0)
				throw new InvalidOperationException("Square restrictions not met");
			
			if (SideA != SideB)
				throw new InvalidOperationException("Square restrictions not met");
		}

		public override double GetArea() => SideA * SideA;
	}
	
	public class Circle : Figure
	{
		public override void Validate()
		{
			if (SideA < 0)
				throw new InvalidOperationException("Circle restrictions not met");
		}

		public override double GetArea() => Math.PI * SideA * SideA;
	}

	public interface IOrderStorage
	{
		// сохраняет оформленный заказ и возвращает сумму

        /* Метод возвращает некое значение. И если бы не комментарии, то непонятно что же это за devimal.
        Либо разбить метод на два (один сохраняет, другой считает сумму), либо обозвать типа "SaveAndCalculateSum" */
		Task<decimal> Save(Order order);
	}
	
	[ApiController]
	[Route("[controller]")]
	public class FiguresController : ControllerBase
	{
		private readonly ILogger<FiguresController> _logger;
		private readonly IOrderStorage _orderStorage;

		public FiguresController(ILogger<FiguresController> logger, IOrderStorage orderStorage)
		{
			_logger = logger;
			_orderStorage = orderStorage;
		}

        // хотим оформить заказ и получить в ответе его стоимость
        /* Если мы делаем REST-сервис, то метод должен выполнять только одну операцию: либо считает стоимость, либо сохраняет заказ. */
		[HttpPost]
		public async Task<ActionResult> Order(Cart cart)
		{
            /* нет проверки на Null свойства cart.Positions */
			foreach (var position in cart.Positions)
			{
                /* проверка бесполезна, так как другой экземпляр может сразу после нашей проверки зарезервировать нужные нам товары */
                /* поэтому здесь нужно использовать один потокобезопасный метод, который сразу резервирует товары (или сообщает, что это невозможно) */
                /* как следствие нужно реализовать метод на отмену резервирования товаров, чтобы в случае ошибки "откатить" товары */
				if (!FiguresStorage.CheckIfAvailable(position.Type, position.Count))
				{
                    /* BadRequest это ошибка 4xx, то есть Client Error. Поэтому такой код тут неуместен, так как сам запрос корректный, и клиент ни в чем не виноват :-) */
                    /* Я не люблю использовать разные HTTP-коды. Уж лучше всегда возвращать ошибку 500, а в теле ответа писать свои кастомные коды ошибок */
                    return new BadRequestResult();
				}
			}

            /* Все валидации лучше производить до резервирования товаров */
			var order = new Order
			{
				Positions = cart.Positions.Select(p =>
				{
                    /* Предлагаю в классе Figure создать метод-фабрику, который принимает Type и возвращает объект нужного класса.*/
					Figure figure = p.Type switch
					{
						"Circle" => new Circle(),
						"Triangle" => new Triangle(),
						"Square" => new Square()
					};
					figure.SideA = p.SideA;
					figure.SideB = p.SideB;
					figure.SideC = p.SideC;

                    /* тут будет исключение. Его желательно перехватить и обработать*/
					figure.Validate();
					return figure;
				}).ToList()
			};

			foreach (var position in cart.Positions)
			{
				FiguresStorage.Reserve(position.Type, position.Count);
			}

            /* Вроде как сумму можно посчитать и без хранилища. Тогда непонятно зачем этим занимается хранилище. */
			var result = _orderStorage.Save(order);

			return new OkObjectResult(result.Result);
		}
	}
}
 
