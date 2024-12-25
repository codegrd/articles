//
//  ConvertToSnakeCase.swift
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
// MARK: - Processing

func runExample4() {
    let user = User(firstName: "Анатолий", lastName: "Свифтозавр")

    let encoder = JSONEncoder()
    encoder.keyEncodingStrategy = .convertToSnakeCase

    let encodedData = try! encoder.encode(user)
    let encodedDataString = String(data: encodedData, encoding: .utf8)!
    
    print("Encoded result = \(encodedDataString)")

}
