//
//  FormattedDate.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Event: Codable
{
    let title: String
    let date: Date
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "title": "Swift Meetup",
    "date": "2024-12-09 15:32:31"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample7() {
    let decoder = JSONDecoder()
    let formatter = DateFormatter()
    formatter.dateFormat = "yyyy-MM-dd HH:mm:ss"
    decoder.dateDecodingStrategy = .formatted(formatter)
    let event = try! decoder.decode(Event.self, from: jsonData)
    print("event.date = \(event.date)")

    let encoder = JSONEncoder()
    encoder.dateEncodingStrategy = .formatted(formatter)
    let encodedData = try! encoder.encode(event)
    let encodedDataString = String(data: encodedData, encoding: .utf8)!
    print("Encoded result = \(encodedDataString)")
}
