//
//  NonConformingFloatDecodingStrategy.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Score: Codable
{
    let value: Double
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "value": "NaN"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample8() {
    let decoder = JSONDecoder()
    decoder.nonConformingFloatDecodingStrategy = .convertFromString(
        positiveInfinity: "Infinity",
        negativeInfinity: "-Infinity",
        nan: "NaN"
    )

    let score = try! decoder.decode(Score.self, from: jsonData)
    print("score.value is NaN: \(score.value.isNaN)")
    print("score.value is infinite: \(score.value.isInfinite)")
}
