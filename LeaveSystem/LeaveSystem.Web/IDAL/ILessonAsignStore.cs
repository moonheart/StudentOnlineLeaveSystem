using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface ILessonAsignStore
    {
        LessonAsign AddLessonAsign(LessonAsign lessonAsign);

        void UpdateLessonAsign(LessonAsign lessonAsign);

        LessonAsign DeleteLessonAsign(LessonAsign lessonAsign);
    }
}
