using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Common Library of methods for the Group Management System.
 * Only includes overhead methods necessary for parent classes to work together, as well ..
 * .. as some methods to populate groups automatically.
 * 
 * - Issues: 
 * No null checks. If a child class (like Grade) is undefined and used in methods, methods return NullReferenceException.
 */

namespace GMS
{
    public class CommonLib
    {
        // This method checks if subjects are part of any group.
        public List<Group> GroupsWithSubjects(List<Subject> subjects, List<Group> grps)
        {
            return (from grp in grps from subject in subjects where grp.Subjects.Contains(subject) select grp).ToList();
        }

        // This method checks if student is part of specified groups.
        public bool IsStudentInGroups(Student student, List<Group> grps)
        {
            return grps.Any(grp => grp.Students.Contains(student));
        }

        // This method adds students to a class. Can be any from 1 to n.
        // It also sets Class variable inside Student to current class.
        // Special condition: IF Student Grade does not match Class Grade, Student will not be able to be assigned to Class.
        public void AddStudentsToClass(List<Student> students, Class classA)
        {
            foreach (var student in students)
            {
                if (student.Grade != classA.Grade)
                {
                    Console.WriteLine("ERROR: Adding Student {0} to Class {1} failed due to different Grades.", 
                        student.StudentName, classA.ClassName);
                    continue;
                };
                classA.Students.Add(student);
                student.Class = classA;
            }
        }

        // This method adds students to a group. Students can be any from 1 to n.
        // It also adds Group to Student class' Group list.
        // This can double as an AddClassToGroup method, as it's possible to simply use Class.Students as first parameter.

        // A few conditions apply here:
        // Student max number may not be reached. If so, student is unplaced.
        // Group Grade must be equivalent to Student Grade.
        // Student may not be part of more than 1 group containing the same subject.

        public void AddStudentsToGroup(List<Student> students, Group grp)
        {
            // Exit immediately if provided number of students is more than allowed in group.
            if (students.Count() > grp.MaxStudents)
            {
                Console.WriteLine(
                    "ERROR: Provided number of students ({0}) exceeds maximum amount of students ({1}) allowed for Group {2}",
                    students.Count(), grp.MaxStudents, grp.GroupName);
                return;
            }
            foreach (var student in students)
            {
                // If grades don't match, go to next iteration.
                if (student.Grade != grp.Grade)
                {
                    Console.WriteLine(
                        "ERROR: Adding Student {0} to Group {1} failed due to different Grades.", 
                        student.StudentName, grp.GroupName);
                    continue;
                };
                // If the group has already been added to student, we put out a warning and go to next iteration.
                if (student.Groups.Contains(grp))
                {
                    Console.WriteLine("ERROR: Student {0} is already a part of Group {1}.",
                        student.StudentName, grp.GroupName);
                    continue;
                }
                // If student is already part of a group containing the same subjects, exit.
                // Null check?
                var subjectGroups = GroupsWithSubjects(student.Subjects, new List<Group>() {grp});
                if (IsStudentInGroups(student, subjectGroups))
                {
                    Console.WriteLine("ERROR: Student {0} is already part of at least one group which contains the same subject as Group {1}.",
                        student.StudentName, grp.GroupName);
                    continue; 
                }
                    grp.Students.Add(student);
                    student.Groups.Add(grp);
            }
        }

        // This method adds a list of Subjects to Student.
        public void AddSubjectsToStudent(List<Subject> subjects, Student student)
        {
            foreach (var subject in subjects)
            {
                // If grades differ, throw out a console error and move to next iteration.
                if (subject.Grade != student.Grade)
                {
                    Console.WriteLine("ERROR: Adding Subject {0} to Student {1} failed due to different Grades.", 
                        subject.SubjectName, student.StudentName);
                    continue;
                };
                // Otherwise, just go on and add.
                student.Subjects.Add(subject);
            }
        }

        // This method adds a list of Subjects to Group.
        // Needs a null check too. Right now if Subjects in Group is undefined, it throws an uncaught error.
        public void AddSubjectsToGroup(List<Subject> subjects, Group grp)
        {
            foreach (var subject in subjects)
            {
                // If grades differ, go to next iteration.
                if (subject.Grade != grp.Grade)
                {
                    Console.WriteLine("ERROR: Adding Subject {0} to Group {1} failed due to different Grades.", 
                        subject.SubjectName, grp.GroupName);
                    continue;
                }
                // If group already contains the subject, next iteration.
                if (grp.Subjects.Contains(subject))
                {
                    Console.WriteLine("ERROR: Group {0} already contains Subject {1}.",
                        grp.GroupName, subject.SubjectName);
                    continue;
                }
                grp.Subjects.Add(subject);
            }
        }

        // This method creates groups with certain input.
        // It automatically looks through eligible students and adds them to the group until max students are reached.
        // If min number of students are not reached, group is created but students are cleared from group.
        public Group PopulateNewGroup(List<Subject> subjects, int minimumStudents, int maximumStudents, List<Class> classNames, List<string> teachers, string grp)
        {
            // This is the proper group filled by values passed onto function.
            var studyGroup = new Group
            {
                Subjects = subjects,
                MinStudents = minimumStudents,
                MaxStudents = maximumStudents,
                Grade = classNames.First().Grade,
                Students = new List<Student>(),
                Teachers = teachers,
                GroupName = grp
            };

            // This is a pretty tedious foreach loop.
            // Iterates through each class, each student in each class, and each subject of each student.
            // Checks if student is eligible for studyGroup.
            foreach (var className in classNames)
            {
                foreach (var student in className.Students)
                {
                    var courses = 0;
                    foreach (var subject in studyGroup.Subjects)
                    {
                        // If the student has the same subject as the study group, courses will add one.
                        if (student.Subjects.Contains(subject)) courses++;
                        // This happens until courses equals the amount of subjects, at which point it'll break the loop.
                        if (courses.Equals(studyGroup.Subjects.Count())) break;
                    }
                    // A second equality check as well as a max check. Will not add student if max number reached.
                    if (courses.Equals(studyGroup.Subjects.Count()) &&
                        studyGroup.Students.Count() < studyGroup.MaxStudents)
                    {
                        // A grade check, before adding student to studyGroup.
                        if (student.Grade.Equals(studyGroup.Grade))
                        {
                            studyGroup.Students.Add(student);
                            student.Groups.Add(studyGroup);
                        }
                    }
                    if (studyGroup.Students.Count() == studyGroup.MaxStudents) break;
                }
            }
            // If the minimum number of students hasn't been reached, the Students List will be cleared and the group will be returned with no students.
            // User will have to manually add students to group in that case.
            if (studyGroup.Students.Count() < studyGroup.MinStudents)
            {
                foreach (var student in studyGroup.Students) student.Groups.Remove(studyGroup);
                studyGroup.Students.Clear();
                Console.WriteLine("ERROR: Group {0} does not have enough students. All students cleared from group. Please add students manually to group.",
                    studyGroup.GroupName);
                return studyGroup;
            }

            // If everything is in order, a proper group with all students is returned.
            if (studyGroup.Students.Count() >= studyGroup.MinStudents &&
                studyGroup.Students.Count() <= studyGroup.MaxStudents)
                return studyGroup;

            // This is fallback point, should never happen as studyGroup Student Count should (or can) never exceed MaxStudents.
            Console.WriteLine("ERROR: Group creator for Group {0} has encountered an error. Null return.",
                studyGroup.GroupName);
            return null;
        }
    }
}
