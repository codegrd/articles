//
//  CustomDateDecoding.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct ExampleData: Decodable
{
    let date: Date
}

// -----------------------------------------------------------------------------
// MARK: - JSONs

private let jsonData1 = """
{
    "date": 1708755795.0
}
""".data(using: .utf8)!

private let jsonData2 = """
{
    "date": "2024-12-24T18:30:00Z"
}
""".data(using: .utf8)!

private let jsonData3 = """
{
    "date": "2014/20/24 18:30"
}
""".data(using: .utf8)!

private let invalidJsonData = """
{
    "date": "abc 2024-12-24 18:30"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample12() {
    let decoder = JSONDecoder()

    decoder.dateDecodingStrategy = .custom { decoder in
        let container = try decoder.singleValueContainer()

        if let timestamp = try? container.decode(Double.self) {
            return Date(timeIntervalSince1970: timestamp)
        } else if let dateString = try? container.decode(String.self) {
            // Попытаемся ISO 8601
            let isoFormatter = ISO8601DateFormatter()
            if let date = isoFormatter.date(from: dateString) {
                return date
            }

            // Попытка fallback на другой формат:
            let formatter = DateFormatter()
            formatter.dateFormat = "yyyy/MM/dd HH:mm"
            if let date = formatter.date(from: dateString) {
                return date
            }

            let errorContext = DecodingError.Context(
                codingPath: decoder.codingPath,
                debugDescription: "Невозможно распарсить дату"
            )
            throw DecodingError.dataCorrupted(errorContext)
        }

        let errorContext = DecodingError.Context(
            codingPath: decoder.codingPath,
            debugDescription: "Невозможно найти дату ни в одном формате"
        )
        throw DecodingError.dataCorrupted(errorContext)
    }

    do {
        let date1 = try decoder.decode(ExampleData.self, from: jsonData1)
        print("Parsed date1: ", date1)
        let date2 = try decoder.decode(ExampleData.self, from: jsonData2)
        print("Parsed date2: ", date2)
        let date3 = try decoder.decode(ExampleData.self, from: jsonData3)
        print("Parsed date3: ", date3)
        let date4 = try decoder.decode(ExampleData.self, from: invalidJsonData)
        print("Parsed date4: ", date4)
    } catch {
        print("Error decoding JSON: ", error)
    }
}
