//
//  UserInfo.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct LocaleDependentValue: Codable
{
    let value: String

    init(from decoder: Decoder) throws {
        let container = try decoder.singleValueContainer()
        let rawValue = try container.decode(String.self)

        let userInfo = decoder.userInfo
        let locale = userInfo[.localeKey] as? Locale ?? Locale.current

        // Допустим, в зависимости от locale делаем что-то с rawValue
        self.value = rawValue + "_\(locale.identifier)"
    }
}

private extension CodingUserInfoKey
{
    static let localeKey = CodingUserInfoKey(rawValue: "locale")!
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let jsonData = "\"Hello\"" .data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample9() {
    let decoder = JSONDecoder()
    decoder.userInfo[.localeKey] = Locale(identifier: "ru_RU")

    let localizedValue = try! decoder.decode(LocaleDependentValue.self, from: jsonData)
    print(localizedValue.value) // "Hello_ru_RU"
}
