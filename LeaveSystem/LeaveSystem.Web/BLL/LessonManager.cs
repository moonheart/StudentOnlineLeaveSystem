using System;
using System.Collections.Generic;
using System.Linq;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    public class LessonManager : IDisposable
    {
        private bool _disposed;

        private LessonStore _lessonStore;

        public LessonManager(LessonStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            _lessonStore = store;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this._lessonStore.Dispose();
            _disposed = true;
        }

        public static LessonManager Create(IdentityFactoryOptions<LessonManager> options, IOwinContext context)
        {
            return new LessonManager(new LessonStore(context.Get<AppDbContext>()));
        }


        public IQueryable<LessonAsign> GetStudentLessons(string userId)
        {
            return _lessonStore.LessonAsigns.Where(e => e.StudentId == userId);
        }
    }
}