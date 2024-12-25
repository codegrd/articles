//
//  InnerStruct.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: - Swift Struct

private struct Author: Codable
{
    let name: String
    let email: String
}

private struct Article: Codable
{
    let title: String
    let author: Author
    let commentsCount: Int
}

// -----------------------------------------------------------------------------
// MARK: - JSON

private let json = """
{
    "title": "Codable: лучшее, что случилось с iOS?",
    "author": {
        "name": "Анатолий Свифтозавр",
        "email": "anatoly@swiftosaurs.com"
    },
    "commentsCount": 7
}
""".data(using: .utf8)!

// -----------------------------------------------------------------------------
// MARK: - Processing

func runExample2() {
    let article = try! JSONDecoder().decode(Article.self, from: json)
    print("article.author.name = \(article.author.name)")
}
