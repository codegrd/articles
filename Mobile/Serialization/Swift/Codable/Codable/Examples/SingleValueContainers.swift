//
//  SingleValueContainers.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = "42".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct JustNumber: Codable 
{
    let value: Int

    init(from decoder: Decoder) throws {
        let container = try decoder.singleValueContainer()
        self.value = try container.decode(Int.self)
    }

    func encode(to encoder: Encoder) throws {
        var container = encoder.singleValueContainer()
        try container.encode(value)
    }
}

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample15() {
    let decoder = JSONDecoder()

    let number = try! decoder.decode(JustNumber.self, from: jsonData)

    print("Just number = \(number)")
}
