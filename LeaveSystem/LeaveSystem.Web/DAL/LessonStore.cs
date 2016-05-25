using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class LessonStore : ILessonInfoStore, ILessonAsignStore, IDisposable
    {

        private EntityStore<LessonInfo> _lessonInfoStore;
        private EntityStore<LessonAsign> _lessonAsignStore;

        public IQueryable<LessonInfo> LessonInfos
        {
            get { return _lessonInfoStore.EntitySet; }
        }
        public IQueryable<LessonAsign> LessonAsigns
        {
            get { return _lessonAsignStore.EntitySet; }
        }
        protected bool DisposeContext { get; set; }

        protected bool Disposed;
        protected DbContext Context { get; private set; }

        public LessonStore(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
            _lessonInfoStore = new EntityStore<LessonInfo>(context);
            _lessonAsignStore = new EntityStore<LessonAsign>(context);
        }


        public LessonInfo AddLessonInfo(LessonInfo lessonInfo)
        {
            ThrowIfDisposed();
            if (lessonInfo == null)
                throw new ArgumentNullException("lessonInfo");
            return _lessonInfoStore.Create(lessonInfo);
        }

        public void UpdateLessonInfo(LessonInfo lessonInfo)
        {
            ThrowIfDisposed();
            if (lessonInfo == null)
                throw new ArgumentNullException("lessonInfo");
            _lessonInfoStore.Update(lessonInfo);
        }

        public LessonInfo DeleteLessonInfo(LessonInfo lessonInfo)
        {
            ThrowIfDisposed();
            if (lessonInfo == null)
                throw new ArgumentNullException("lessonInfo");
            return _lessonInfoStore.Delete(lessonInfo);
        }

        public LessonAsign AddLessonAsign(LessonAsign lessonAsign)
        {
            ThrowIfDisposed();
            if (lessonAsign == null)
                throw new ArgumentNullException("lessonAsign");
            return _lessonAsignStore.Create(lessonAsign);
        }

        public void UpdateLessonAsign(LessonAsign lessonAsign)
        {
            ThrowIfDisposed();
            if (lessonAsign == null)
                throw new ArgumentNullException("lessonAsign");
            _lessonAsignStore.Update(lessonAsign);
        }

        public LessonAsign DeleteLessonAsign(LessonAsign lessonAsign)
        {
            ThrowIfDisposed();
            if (lessonAsign == null)
                throw new ArgumentNullException("lessonAsign");
            return _lessonAsignStore.Delete(lessonAsign);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);

        }

        public void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
                this.Context.Dispose();
            this.Disposed = true;
            this.Context = (DbContext)null;
            this._lessonInfoStore = null;
            this._lessonAsignStore = null;
        }
        protected void ThrowIfDisposed()
        {
            if (this.Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
    }
}