//
//  DecodingError.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct User: Codable
{
    let name: String
    let age: Int
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "name": "Анатолий",
    "age": "notANumber"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample18() {
    do {
        let _ = try JSONDecoder().decode(User.self, from: jsonData)
    } catch let error as DecodingError {
        switch error {
        case let .typeMismatch(type, context):
            print("Ошибка типа: ожидается \(type), путь: \(context.codingPath), описание: \(context.debugDescription)")
        case let .valueNotFound(type, context):
            print("Значение не найдено для \(type), путь: \(context.codingPath), описание: \(context.debugDescription)")
        case let .keyNotFound(key, context):
            print("Ключ не найден: \(key), путь: \(context.codingPath), описание: \(context.debugDescription)")
        case let .dataCorrupted(context):
            print("Данные искажены: путь: \(context.codingPath), описание: \(context.debugDescription)")
        @unknown default:
            print("Неизвестная ошибка декодирования")
        }
    } catch {
        print("Другая ошибка: \(error)")
    }
}
