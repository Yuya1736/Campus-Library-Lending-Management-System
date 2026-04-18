CREATE DATABASE IF NOT EXISTS campus_library CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE campus_library;

CREATE TABLE IF NOT EXISTS book (
    isbn VARCHAR(32) PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    author VARCHAR(100) NOT NULL,
    publisher VARCHAR(100) NOT NULL,
    category VARCHAR(100) NOT NULL,
    stock_qty INT NOT NULL DEFAULT 0,
    borrow_count INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS student (
    student_id VARCHAR(32) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    class_name VARCHAR(100) NOT NULL,
    contact VARCHAR(50) NOT NULL,
    total_borrow_count INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(128) NOT NULL,
    role VARCHAR(20) NOT NULL,
    last_login_time DATETIME NULL
);

CREATE TABLE IF NOT EXISTS borrow_record (
    record_id INT AUTO_INCREMENT PRIMARY KEY,
    isbn VARCHAR(32) NOT NULL,
    student_id VARCHAR(32) NOT NULL,
    borrow_date DATETIME NOT NULL,
    due_date DATETIME NOT NULL,
    return_date DATETIME NULL,
    status VARCHAR(20) NOT NULL,
    overdue_days INT NOT NULL DEFAULT 0,
    CONSTRAINT fk_borrow_book FOREIGN KEY (isbn) REFERENCES book(isbn),
    CONSTRAINT fk_borrow_student FOREIGN KEY (student_id) REFERENCES student(student_id)
);

CREATE TABLE IF NOT EXISTS operation_log (
    log_id BIGINT AUTO_INCREMENT PRIMARY KEY,
    operator_username VARCHAR(50) NOT NULL,
    operation_type VARCHAR(50) NOT NULL,
    content VARCHAR(500) NOT NULL,
    operation_time DATETIME NOT NULL,
    ip_address VARCHAR(50) NOT NULL
);

