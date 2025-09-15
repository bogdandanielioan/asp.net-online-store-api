
-- Seed data for single string Id schema (no PublicId columns)

-- Define deterministic string IDs for cross references
-- Students
INSERT INTO students (Id, Name, Email, Age, UpdateDate) VALUES
  ('onl-stu-1', 'GABI', 'gabi@gmail.com', 17, '2024-05-11 18:49:00'),
  ('onl-stu-2', 'ana', 'ana@gmail.com', 19, '2024-05-11 18:49:00'),
  ('onl-stu-3', 'filip', 'filip@gmail.com', 16, '2024-05-11 18:49:00');

-- Books
INSERT INTO books (Id, IdStudent, Name, Created) VALUES
  ('onl-book-1', 'onl-stu-2', 'test', '2024-04-20'),
  ('onl-book-2', 'onl-stu-1', 'test1', '2024-04-21'),
  ('onl-book-3', 'onl-stu-3', 'test2', '2024-04-21');

-- Student cards
INSERT INTO studentscard (Id, IdStudent, Namecard) VALUES
  ('onl-card-1', 'onl-stu-1', '234235246'),
  ('onl-card-2', 'onl-stu-2', '325346357'),
  ('onl-card-3', 'onl-stu-3', '457453765');

-- Courses
INSERT INTO courses (Id, Name, Department) VALUES
  ('onl-course-1', 'test', 'testDpt'),
  ('onl-course-2', 'test1', 'testDpt1'),
  ('onl-course-3', 'test2', 'testDpt1');

-- Enrolments
INSERT INTO enrolments (Id, IdCourse, IdStudent, Created) VALUES
  ('onl-enr-1', 'onl-course-3', 'onl-stu-2', '2024-04-25 00:00:00'),
  ('onl-enr-2', 'onl-course-2', 'onl-stu-2', '2024-04-24 00:00:00'),
  ('onl-enr-3', 'onl-course-1', 'onl-stu-3', '2024-04-23 00:00:00');

