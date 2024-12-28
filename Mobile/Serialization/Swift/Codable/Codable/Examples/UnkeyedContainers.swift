//
//  UnkeyedContainers.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
["Hello", "Tag2", "World"]
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Tags: Codable 
{
    let tags: [String]

    init(from decoder: Decoder) throws {
        var container = try decoder.unkeyedContainer()
        var result: [String] = []

        while !container.isAtEnd {
            let tag = try container.decode(String.self)
            result.append(tag)
        }
        self.tags = result
    }

    func encode(to encoder: Encoder) throws {
        var container = encoder.unkeyedContainer()
        for tag in tags {
            try container.encode(tag)
        }
    }
}

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample16() {
    let decoder = JSONDecoder()

    let tags = try! decoder.decode(Tags.self, from: jsonData)

    print("All tags = \(tags)")
}
