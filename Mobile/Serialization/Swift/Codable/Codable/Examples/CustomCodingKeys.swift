//
//  MixedValueTypes.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct WeirdUser: Codable
{
    let firstName: String
    let lastName: String

    private enum CodingKeys: String, CodingKey 
    {
        case firstName = "Fn"
        case lastName = "Last_Name"
    }
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "Fn": "Анатолий",
    "Last_Name": "Свифтозавр"
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample11() {
    let decoder = JSONDecoder()

    let user = try! decoder.decode(WeirdUser.self, from: jsonData)

    print(user.firstName)
    print(user.lastName)
}
