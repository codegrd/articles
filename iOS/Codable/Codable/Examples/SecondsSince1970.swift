//
//  SecondsSince1970.swift
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
    "date": 1735076997
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample5() {
    let decoder = JSONDecoder()
    decoder.dateDecodingStrategy = .secondsSince1970
    let event = try! decoder.decode(Event.self, from: jsonData)
    print("event.date = \(event.date)")

    let encoder = JSONEncoder()
    encoder.dateEncodingStrategy = .secondsSince1970
    let encodedData = try! encoder.encode(event)
    let encodedDataString = String(data: encodedData, encoding: .utf8)!
    print("Encoded result = \(encodedDataString)")

}
