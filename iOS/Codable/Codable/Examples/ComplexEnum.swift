//
//  ComplexEnum.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Entities

private struct Meeting: Codable
{
    let title: String
    let attendees: Int
}

private struct Party: Codable
{
    let title: String
    let djName: String
}

private enum Event: Codable
{
    case meeting(Meeting)
    case party(Party)

    init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        let type = try container.decode(String.self, forKey: .type)

        switch type {
        case "meeting":
            let meeting = try Meeting(from: decoder)
            self = .meeting(meeting)
        case "party":
            let party = try Party(from: decoder)
            self = .party(party)
        default:
            throw DecodingError.dataCorrupted(DecodingError.Context(
                codingPath: decoder.codingPath,
                debugDescription: "Неизвестный тип события: \(type)"
            ))
        }
    }

    func encode(to encoder: Encoder) throws {
        switch self {
        case .meeting(let meeting):
            try meeting.encode(to: encoder)
        case .party(let party):
            try party.encode(to: encoder)
        }
    }

    private enum CodingKeys: String, CodingKey
    {
        case type
    }
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = """
[
  { "type": "meeting", "title": "Standup", "attendees": 5 },
  { "type": "party", "title": "Birthday Bash", "djName": "DJ Swift" }
]
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample13() {
    let decoder = JSONDecoder()
    
    let events = try! decoder.decode([Event].self, from: jsonData)

    for event in events {
        switch event {
        case .meeting(let m):
            print("Meeting: \(m.title), Attendees: \(m.attendees)")
        case .party(let p):
            print("Party: \(p.title), DJ: \(p.djName)")
        }
    }

}
