//
//  Simple.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Product: Codable
{
    let id: Int
    let name: String
    let price: Double
    let tags: [String]
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "id": 42,
    "name": "iPhone Ultra Max Pro Turbo",
    "price": 9999.99,
    "tags": ["phone", "apple", "premium"]
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample1() {
    do {
        let product = try JSONDecoder().decode(Product.self, from: jsonData)
        print("product.name = \(product.name)")

        let encodedData = try JSONEncoder().encode(product)
        let encodedDataString = String(data: encodedData, encoding: .utf8)!
        print("Encoded result = \(encodedDataString)")
    } catch {
        print("Произошла ошибка: \(error)")
    }
}
