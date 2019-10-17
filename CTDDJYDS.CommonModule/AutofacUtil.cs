public class AutofacUtil
    {
        /// <summary>
        /// Autofac容器对象
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// 初始化autofac
        /// </summary>
        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            //配置接口依赖
            builder.RegisterInstance<IDbConnection>(DBFactory.CreateConnection()).As<IDbConnection>();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));

            //注入仓储类
            builder.RegisterAssemblyTypes(Assembly.Load("Demo.Repository"))
                   .Where(x => x.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();

            //配置quartz.net依赖注入
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));

            _container = builder.Build();
        }

        /// <summary>
        /// 从Autofac容器获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }