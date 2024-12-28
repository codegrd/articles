//
//  ConvertFromSnakeCase.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct User: Codable 
{
    let firstName: String
    let lastName: String
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "first_name": "Анатолий",
    "last_name": "Свифтозавр"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample3() {
    let decoder = JSONDecoder()
    decoder.keyDecodingStrategy = .convertFromSnakeCase

    let user = try! decoder.decode(User.self, from: jsonData)
    
    print("user.firstName = \(user.firstName)")
    print("user.lastName = \(user.lastName)")
}
