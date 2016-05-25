﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Common
{
    public enum Charactor
    {
        Administrator,
        Teacher,
        Student,
        Guest
    }

    public abstract class UserNumberTool
    {
        public static Charactor IsTeacherOrStudent(string number)
        {
            number = number.ToUpper();
            if (number.StartsWith("A"))
                return Charactor.Administrator;
            if (number.StartsWith("T"))
                return Charactor.Teacher;
            if (number.StartsWith("S"))
                return Charactor.Student;
            return Charactor.Guest;
        }
    }
}