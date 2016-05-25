using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface ILessonInfoStore
    {
        LessonInfo AddLessonInfo(LessonInfo lessonInfo);

        void UpdateLessonInfo(LessonInfo lessonInfo);

        LessonInfo DeleteLessonInfo(LessonInfo lessonInfo);

    }
}
