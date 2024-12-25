//
//  DynamicKeys.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Entities

private struct Product: Codable
{
    let name: String
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
  "product_42": { "name": "iPhone" },
  "product_99": { "name": "MacBook" }
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample14() {
    let decoder = JSONDecoder()

    // ["product_42": Product(name: "iPhone"), "product_99": Product(name: "MacBook")]
    let container = try! decoder.decode([String: Product].self, from: jsonData)

    var productsByID: [Int: Product] = [:]
    for (key, product) in container {
        if let idString = key.split(separator: "_").last, let id = Int(idString) {
            productsByID[id] = product
        }
    }

    print("productsByID[42] = \(productsByID[42]?.name ?? "nil")")
    print("productsByID[99] = \(productsByID[99]?.name ?? "nil")")
    print("productsByID[3] = \(productsByID[3]?.name ?? "nil")")
}
