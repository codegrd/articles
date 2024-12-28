//
//  NestedContainer.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
{
    "data": {
        "users": [
            {
                "id": 1,
                "name": "Анатолий"
            },
            {
                "id": 2,
                "name": "Свифтозавр"
            }
        ]
    }
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Response: Decodable
{
    let users: [User]

    struct User: Codable 
    {
        let id: Int
        let name: String
    }

    init(from decoder: Decoder) throws {
        let container = try decoder.container(
            keyedBy: CodingKeys.self
        )
        let dataContainer = try container.nestedContainer(
            keyedBy: DataKeys.self,
            forKey: .data
        )
        var usersContainer = try dataContainer.nestedUnkeyedContainer(
            forKey: .users
        )

        var usersList: [User] = []
        while !usersContainer.isAtEnd {
            let user = try usersContainer.decode(User.self)
            usersList.append(user)
        }
        self.users = usersList
    }

    private enum CodingKeys: String, CodingKey
    {
        case data
    }

    private enum DataKeys: String, CodingKey 
    {
        case users
    }
}

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample17() {
    let decoder = JSONDecoder()

    let response = try! decoder.decode(Response.self, from: jsonData)

    print("Response = \(response)")
}
