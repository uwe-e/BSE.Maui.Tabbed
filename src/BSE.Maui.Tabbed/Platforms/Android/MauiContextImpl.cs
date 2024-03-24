using Android.Content;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AContext = Android.Content.Context;

namespace BSE.Maui.Tabbed.Platforms
{
    public class MauiContextImpl : IMauiContext
    {
        readonly WrappedServiceProvider _services;
        readonly Lazy<IMauiHandlersFactory> _handlers;
        readonly Lazy<AContext?> _context;

        public AContext? Context => _context.Value;

        public IServiceProvider Services => _services;

        public IMauiHandlersFactory Handlers => _handlers.Value;

        public MauiContextImpl(IServiceProvider services)
        {
            _services = new WrappedServiceProvider(services);

            _handlers = new Lazy<IMauiHandlersFactory>(() => _services.GetRequiredService<IMauiHandlersFactory>());

            _context = new Lazy<AContext?>(() => _services.GetService<AContext>());
        }

        internal void AddSpecific<TService>(TService instance)
            where TService : class
        {
            _services.AddSpecific(typeof(TService), static state => state, instance);
        }

        internal void AddWeakSpecific<TService>(TService instance)
            where TService : class
        {
            _services.AddSpecific(typeof(TService), static state => ((WeakReference)state).Target, new WeakReference(instance));
        }

        class WrappedServiceProvider : IServiceProvider
        {
            readonly ConcurrentDictionary<Type, (object, Func<object, object?>)> _scopeStatic = new();

            public WrappedServiceProvider(IServiceProvider serviceProvider)
            {
                Inner = serviceProvider;
            }

            public IServiceProvider Inner { get; }

            public object? GetService(Type serviceType)
            {
                if (_scopeStatic.TryGetValue(serviceType, out var scope))
                {
                    var (state, getter) = scope;
                    return getter.Invoke(state);
                }

                return Inner.GetService(serviceType);
            }

            public void AddSpecific(Type type, Func<object, object?> getter, object state)
            {
                _scopeStatic[type] = (state, getter);
            }
        }
    }
}
