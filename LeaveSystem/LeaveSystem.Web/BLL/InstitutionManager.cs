using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebGrease.Css.Extensions;

namespace LeaveSystem.Web.BLL
{
    public class InstitutionManager : IDisposable
    {
        private bool _disposed;

        private InstitutionStore Store { get; set; }

        public InstitutionManager(InstitutionStore institutionStore)
        {
            if (institutionStore == null)
                throw new ArgumentNullException("institutionStore");
            this.Store = institutionStore;
        }

        #region Common Method
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this._disposed)
                return;
            this.Store.Dispose();
            this._disposed = true;
        }

        public static InstitutionManager Create(IdentityFactoryOptions<InstitutionManager> options, IOwinContext context)
        {
            return new InstitutionManager(new InstitutionStore(context.Get<AppDbContext>()));
        }

        #endregion

        #region Department
        private IDepartmentStore GetDepartmentStore()
        {
            var departmentStore = this.Store as IDepartmentStore;
            if (departmentStore == null)
                throw new NotSupportedException();
            return departmentStore;
        }

        public async Task<ICollection<Department>> GetAllDepartmentsAsync()
        {
            ThrowIfDisposed();
            var store = GetDepartmentStore();
            return await store.GetAllDepartmentsAsync();
        }

        public async Task<Department> FindDepartmentByNameAsync(string name)
        {
            ThrowIfDisposed();
            var store = GetDepartmentStore();
            return await store.FindDepartmentByNameAsync(name);
        }

        public async Task<Department> FindDepartmentByIdAsync(int id)
        {
            ThrowIfDisposed();
            var store = GetDepartmentStore();
            return await store.GetDepartmentByIdAsync(id);
        }

        public async Task AddDepartmentAsync(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var store = GetDepartmentStore();
            var de = await store.FindDepartmentByNameAsync(name);
            if (de != null)
                throw new Exception("已存在");
            var d = new Department { Name = name };
            await store.AddDepartmentAsync(d);
        }

        public async Task UpdateDepartmentAsync(Department de)
        {
            ThrowIfDisposed();
            if (de == null)
                throw new ArgumentNullException("de");

            var store = GetDepartmentStore();
            var instance = await store.GetDepartmentByIdAsync(de.Id);
            if (instance == null)
                throw new Exception("找不到");
            await store.UpdateDepartmentAsync(de);
        }

        public async Task DeleteDepartmentAsync(Department de)
        {
            ThrowIfDisposed();
            if (de == null)
                throw new ArgumentNullException("de");

            var store = GetDepartmentStore();
            var instance = await store.GetDepartmentByIdAsync(de.Id);
            if (instance == null)
                throw new Exception("找不到");
            instance.IsDeleted = true;
            await store.UpdateDepartmentAsync(de);
        }

        public async Task<IdentityResult> AddMajorsToDepartmentAsync(Department department, int[] ids)
        {
            if (department == null)
                throw new ArgumentNullException("department");
            if (ids == null)
                throw new ArgumentNullException("ids");
            var dstore = GetDepartmentStore();
            var mStore = GetMajorStore();
            var majors = new List<Major>();
            foreach (var id in ids)
            {
                majors.Add(await mStore.FindMajorByIdAsync(id));
            }
            majors.ForEach(m => department.Majors.Add(m));

            await dstore.UpdateDepartmentAsync(department);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RemoveMajorsFromDepartmentAsync(Department department, int[] ids)
        {
            if (department == null)
                throw new ArgumentNullException("department");
            if (ids == null)
                throw new ArgumentNullException("ids");
            var dstore = GetDepartmentStore();
            var mStore = GetMajorStore();
            var majors = new List<Major>();
            foreach (var id in ids)
            {
                majors.Add(await mStore.FindMajorByIdAsync(id));
            }
            majors.ForEach(m => department.Majors.Remove(m));

            await dstore.UpdateDepartmentAsync(department);
            return IdentityResult.Success;
        }

        #endregion

        #region Major

        private IMajorStore GetMajorStore()
        {
            var majorStore = Store as IMajorStore;
            if (majorStore == null)
                throw new NotSupportedException();
            return majorStore;
        }

        public async Task AddMajorAsync(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var store = GetMajorStore();
            var de = await store.FindMajorByNameAsync(name);
            if (de != null)
                throw new Exception("已存在");
            var d = new Major { Name = name };
            await store.CreateMajorAsync(d);
        }

        public async Task<ICollection<Major>> GetAllMajorsAsync()
        {
            ThrowIfDisposed();
            var store = GetMajorStore();
            return await store.GetAllMajorsAsync();
        }

        public async Task<Major> FindMajorByNameAsync(string name)
        {
            ThrowIfDisposed();
            var store = GetMajorStore();
            return await store.FindMajorByNameAsync(name);
        }

        public async Task<Major> FindMajorByIdAsync(int id)
        {
            ThrowIfDisposed();
            var store = GetMajorStore();
            return await store.FindMajorByIdAsync(id);
        }

        public async Task<List<Major>> GetMajorsForDepartmentAsync(Department de)
        {
            ThrowIfDisposed();
            var store = GetMajorStore();
            return await store.FindMajorsOfDepartment(de);
        }

        public async Task<List<Major>> GeNoDepartmentMajorsAsync(Department de)
        {
            ThrowIfDisposed();

            return await Store.FindAllMajorsExceptAsync(de);
        }

        public async Task UpdateMajorAsync(Major major)
        {
            ThrowIfDisposed();
            var store = GetMajorStore();
            await store.UpdateMajorAsync(major);
        }

        public async Task DeleteMajorAsync(Major major)
        {
            ThrowIfDisposed();
            if (major == null)
                throw new ArgumentNullException("major");
            major.IsDeleted = true;
            var store = this.GetMajorStore();
            await store.UpdateMajorAsync(major);
        }

        #endregion

        #region Grade

        private IGradeStore GetGradeStore()
        {
            var store = Store as IGradeStore;
            if (store == null)
                throw new NotSupportedException();
            return store;
        }

        public async Task<ICollection<Grade>> GetAllGradesAsync()
        {
            ThrowIfDisposed();
            var store = GetGradeStore();
            return await store.GetAllGradeAsync();
        }

        public async Task<Grade> FindGradeByIdAsync(int id)
        {
            ThrowIfDisposed();
            var store = GetGradeStore();
            return await store.FindGradeByIdAsync(id);
        }

        public async Task<Grade> FindGradeByGradeNumberAsync(int num)
        {
            ThrowIfDisposed();
            var store = GetGradeStore();
            return await store.FindGradeByNumberAsync(num);
        }

        public async Task AddGradeAsync(int num)
        {
            ThrowIfDisposed();
            var store = GetGradeStore();
            var grade = await store.FindGradeByNumberAsync(num);
            if (grade != null)
                throw new Exception("grade already exist!");
            var instance = new Grade { GradeNum = num };
            await store.CreateGradeAsync(instance);
        }

        public async Task DeleteGradeAsync(Grade grade)
        {
            ThrowIfDisposed();
            if (grade == null)
                throw new ArgumentNullException("grade");
            grade.IsDeleted = true;
            var store = GetGradeStore();
            await store.UpdateGradeAsync(grade);
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            ThrowIfDisposed();
            if (grade == null)
                throw new ArgumentNullException("grade");
            var store = GetGradeStore();
            await store.UpdateGradeAsync(grade);
        }


        #endregion

        #region Class

        private IClassStore GetClassStore()
        {
            var store = Store as IClassStore;
            if (store == null)
                throw new NotSupportedException();
            return store;
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            ThrowIfDisposed();
            var store = GetClassStore();
            return await store.GetAllClassesAsync();
        }

        public async Task AddClassAsync(Class class1)
        {
            ThrowIfDisposed();
            var store = GetClassStore();
            await store.AddClassAsync(class1);
        }

        public async Task<bool> IsClassExistAsync(Expression<Func<Class, bool>> lamda)
        {
            ThrowIfDisposed();
            var store = GetClassStore();
            return await store.IsClassExistAsync(lamda);
        }

        public async Task<Class> FindClassByIdAsync(int id)
        {
            ThrowIfDisposed();
            var store = GetClassStore();
            return await store.FindClassById(id);
        }

        public async Task UpdateClassAsync(Class class1)
        {
            ThrowIfDisposed();
            if (class1 == null)
                throw new ArgumentNullException("class1");
            var store = GetClassStore();
            await store.UpdateClassAsync(class1);
        }

        public async Task DeleteClassAsync(Class class1)
        {
            ThrowIfDisposed();
            if (class1 == null)
                throw new ArgumentNullException("class1");
            var store = GetClassStore();
            class1.IsDeleted = true;
            await store.UpdateClassAsync(class1);
        }
        #endregion



    }
}