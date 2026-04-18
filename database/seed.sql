USE campus_library;

DELETE FROM borrow_record;
DELETE FROM operation_log;
DELETE FROM user;
DELETE FROM student;
DELETE FROM book;

INSERT INTO book(isbn, title, author, publisher, category, stock_qty, borrow_count) VALUES
('9787302511850', 'C# 程序设计基础', '张三', '清华大学出版社', '计算机', 6, 0),
('9787115545387', '数据库系统概论', '李四', '人民邮电出版社', '数据库', 4, 0),
('9787302601292', '软件工程导论', '王五', '高等教育出版社', '软件工程', 5, 0);

INSERT INTO student(student_id, name, class_name, contact, total_borrow_count) VALUES
('20230001', '陈一', '计科2301', '13800001111', 0),
('20230002', '刘二', '计科2302', '13800002222', 0),
('20230003', '周三', '软工2301', '13800003333', 0);

-- 密码明文分别为：123456 / operator123（SHA256）
INSERT INTO user(username, password_hash, role) VALUES
('admin', '8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', '管理员'),
('operator', 'EC6E1C25258002EB1C67D15C7F45DA7945FA4C58778FD7D88FAA5E53E3B4698D', '操作员');

