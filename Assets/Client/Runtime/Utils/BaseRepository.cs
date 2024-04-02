using System;
using System.Collections.Generic;

using UnityEngine.Pool;

namespace Client.Runtime.Utils
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        private ObjectPool<T> _pool;
        private Func<T> _createFunc;

        protected BaseRepository(Func<T> createFunc)
        {
            _createFunc = createFunc;
        }

        public List<T> ActiveElements { get; private set; } = new();

        public T Get()
        {
            _pool ??= CreatePool();
            var element = _pool.Get();
            ActiveElements.Add(element);
            OnGet(element);
            return element;
        }

        public void Release(T model)
        {
            ActiveElements.Remove(model);
            if (!model.Active)
                return;
            _pool.Release(model);
            OnRelease(model);
        }

        protected virtual void OnGet(T model)
        {
        }

        protected virtual void OnRelease(T model)
        {
        }

        private ObjectPool<T> CreatePool()
        {
            _pool = new ObjectPool<T>(() =>
            {
                var model = _createFunc();
                return model;
            }, model => { model.Active = true; }, model => { model.Active = false; });

            return _pool;
        }
    }
}