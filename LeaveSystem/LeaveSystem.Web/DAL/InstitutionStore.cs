using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class InstitutionStore 
        //IDepartmentStore,
        //IMajorStore,
        //IGradeStore,
        //IClassStore,
        //IOfficeStore,
        //IPositionStore
    {
        public bool DisposeContext { get; set; }
        public bool AutoSaveChanges { get; set; }

        //private IDbSet<Class> _classes;
        //private IDbSet<Department> _departments;
        //private IDbSet<Grade> _grades;
        //private IDbSet<Major> _majors;
        private EntityStore<Class> _classStore;
        private EntityStore<Department> _departmentStore;
        private EntityStore<Grade> _gradeStore;
        private EntityStore<Major> _majorStore;
        private EntityStore<Office> _officeStore;
        private EntityStore<Position> _positionStore;

        private bool _disposed;
        public DbContext Context { get; private set; }

        public InstitutionStore(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.Context = context;
            this._classStore = new EntityStore<Class>(context);
            this._departmentStore = new EntityStore<Department>(context);
            this._gradeStore = new EntityStore<Grade>(context);
            this._majorStore = new EntityStore<Major>(context);
            _officeStore = new EntityStore<Office>(context);
            _officeStore = new EntityStore<Office>(context);
            this.AutoSaveChanges = true;
            //this._classes = Context.Set<Class>();
            //this._departments = Context.Set<Department>();
            //this._grades = Context.Set<Grade>();
            //_majors = Context.Set<Major>();
        }

        #region Department

        public async Task AddDepartmentAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException("department");
            _departmentStore.Create(department);
            await SaveChanges();
        }

        public async Task<Department> FindDepartmentByNameAsync(string departmentName)
        {
            return await _departmentStore.EntitySet.Where(d => !d.IsDeleted && d.Name == departmentName).SingleOrDefaultAsync();
        }

        public Task<Department> GetDepartmentByIdAsync(int id)
        {
            return _departmentStore.EntitySet.SingleOrDefaultAsync(d => !d.IsDeleted && d.Id == id);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _departmentStore.Update(department);
            await SaveChanges();
        }

        public async Task<ICollection<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentStore.EntitySet.Where(d => !d.IsDeleted).ToListAsync();
        }


        #endregion

        #region Major

        public async Task CreateMajorAsync(Major major)
        {
            if (major == null)
                throw new ArgumentNullException("major");
            _majorStore.Create(major);
            await SaveChanges();
        }

        public async Task RemoveMajorAsync(Major major)
        {
            if (major == null)
                throw new ArgumentNullException("major");
            _majorStore.Delete(major);
            await SaveChanges();
        }

        public async Task<Major> FindMajorByIdAsync(int id)
        {
            return await _majorStore.EntitySet.SingleOrDefaultAsync(m => !m.IsDeleted && m.Id == id);
        }

        public async Task<Major> FindMajorByNameAsync(string name)
        {
            return await _majorStore.EntitySet.SingleOrDefaultAsync(m => !m.IsDeleted && m.Name == name);
            //return await _majors.SingleOrDefaultAsync(m => !m.IsDeleted && m.Name == name);
        }

        public async Task UpdateMajorAsync(Major major)
        {
            if (major == null)
                throw new ArgumentNullException("major");
            _majorStore.Update(major);
            //_majors.Attach(major);
            await SaveChanges();
        }

        public Task<List<Major>> FindMajorsOfDepartmentAsync(Department department)
        {
            return _majorStore.EntitySet.Where(m => !m.IsDeleted && m.Department.Id == department.Id).ToListAsync();
        }

        public async Task<List<Major>> FindAllMajorsExceptAsync(Department de)
        {
            return await _majorStore.EntitySet.Where(m => !m.IsDeleted && m.Department.Id != de.Id).ToListAsync();
        }

        public async Task<List<Major>> GetAllMajorsAsync()
        {
            return await _majorStore.EntitySet.Where(m => !m.IsDeleted).ToListAsync();
        }

        #endregion

        #region Grade

        public async Task CreateGradeAsync(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException("grade");
            _gradeStore.Create(grade);
            await SaveChanges();
        }

        public async Task<Grade> FindGradeByIdAsync(int gradeId)
        {
            return await _gradeStore.EntitySet.SingleOrDefaultAsync(g => !g.IsDeleted && g.Id == gradeId);
        }

        public async Task<Grade> FindGradeByNumberAsync(int num)
        {
            return await _gradeStore.EntitySet.SingleOrDefaultAsync(g => !g.IsDeleted && g.GradeNum == num);
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException("grade");
            _gradeStore.Update(grade);
            await SaveChanges();
        }

        public async Task<ICollection<Grade>> GetAllGradeAsync()
        {
            return await _gradeStore.EntitySet.Where(g => !g.IsDeleted).ToListAsync();
        }


        #endregion

        #region Class

        public async Task AddClassAsync(Class class1)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            _classStore.Create(class1);
            await SaveChanges();
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await _classStore.EntitySet.Where(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<Class> FindClassById(int classId)
        {
            return await _classStore.EntitySet.SingleOrDefaultAsync(c => !c.IsDeleted && c.Id == classId);
        }

        public async Task UpdateClassAsync(Class class1)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            _classStore.Update(class1);
            await SaveChanges();
        }

        public async Task<bool> IsClassExistAsync(Expression<Func<Class, bool>> lamda)
        {
            if (lamda == null)
                throw new ArgumentNullException("lamda");
            return await _classStore.EntitySet.AnyAsync(lamda);
        }

        #endregion

        #region Common Method

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private async Task SaveChanges()
        {
            if (this.AutoSaveChanges)
            {
                int num = await this.Context.SaveChangesAsync();
            }
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        private void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
                this.Context.Dispose();
            this._disposed = true;
            this.Context = (DbContext)null;
            this._departmentStore = (EntityStore<Department>)null;
        }

        #endregion

        //#region Office
        //public async Task CreateOfficeAsync(Office office)
        //{
        //    if (office == null)
        //        throw new ArgumentNullException("office");
        //    _officeStore.Create(office);
        //    await SaveChanges();
        //}

        //public async Task UpdateOfficeAsync(Office office)
        //{
        //    if (office == null)
        //        throw new ArgumentNullException("office");
        //    _officeStore.Update(office);
        //    await SaveChanges();
        //}

        //public Task<Office> FindOfficeByIdAsync(int id)
        //{
        //    return _officeStore.EntitySet.SingleOrDefaultAsync(o => !o.IsDeleted && o.Id == id);
        //}

        //public Task<Office> FindOfficeByNameAsync(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IQueryable<Office>> GetAllOfficesAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion


        #region Position
        public Task CreatePositionAsync(Position position)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOfficeAsync(Position position)
        {
            throw new NotImplementedException();
        }

        Task<Position> IPositionStore.FindOfficeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Position> IPositionStore.FindOfficeByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<IQueryable<Position>> IPositionStore.GetAllOfficesAsync()
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}