//
//  MixedValueTypes.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct FlexibleValue: Codable
{
    let value: String

    init(from decoder: Decoder) throws {
        let container = try decoder.singleValueContainer()

        if let intVal = try? container.decode(Int.self) {
            self.value = String(intVal)
        } else if let strVal = try? container.decode(String.self) {
            self.value = strVal
        } else {
            throw DecodingError.typeMismatch(
                String.self,
                DecodingError.Context(
                    codingPath: decoder.codingPath,
                    debugDescription: "Неподдерживаемый тип"
                )
            )
        }
    }
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
["Hello", 123, "World", 9999]
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample10() {
    let values = try! JSONDecoder().decode([FlexibleValue].self, from: jsonData)
    print(values.map { $0.value })
}
