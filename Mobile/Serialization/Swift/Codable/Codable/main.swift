//
//  main.swift
//  Codable
//
//  Created by Codegrad on 25.12.2024.
//  Copyright © 2024 Codegrad. All rights reserved.

import Foundation

// -----------------------------------------------------------------------------
// MARK: Examples

printSeparator(exampleName: "Пример 1. Базовый")
runExample1()

printSeparator(exampleName: "Пример 2. Вложенные структуры")
runExample2()

printSeparator(exampleName: "Пример 3. Декодирование с convertFromSnakeCase")
runExample3()

printSeparator(exampleName: "Пример 4. Кодирование с convertToSnakeCase")
runExample4()

printSeparator(exampleName: "Пример 5. dateDecodingStrategy = .secondsSince1970")
runExample5()

printSeparator(exampleName: "Пример 6. dateDecodingStrategy = .iso8601")
runExample6()

printSeparator(exampleName: "Пример 7. dateDecodingStrategy = .formatted")
runExample7()

printSeparator(exampleName: "Пример 8. nonConformingFloatDecodingStrategy")
runExample8()

printSeparator(exampleName: "Пример 9. userInfo")
runExample9()

printSeparator(exampleName: "Пример 10. Разные типы внутри массива")
runExample10()

printSeparator(exampleName: "Пример 11. Custom CodingKeys")
runExample11()

printSeparator(exampleName: "Пример 12. Custom Date Decoding")
runExample12()

printSeparator(exampleName: "Пример 13. Enum with associated values")
runExample13()

printSeparator(exampleName: "Пример 14. Динамические ключи в JSON")
runExample14()

printSeparator(exampleName: "Пример 15. singleValueContainer")
runExample15()

printSeparator(exampleName: "Пример 16. unkeyedContainer")
runExample16()

printSeparator(exampleName: "Пример 17. nestedContainer и nestedUnkeyedContainer")
runExample17()

printSeparator(exampleName: "Пример 18. DecodingError")
runExample18()

printSeparator()

// -----------------------------------------------------------------------------
// MARK: Private Helpers

private func printSeparator(exampleName: String = "") {
    print("\n\(String(repeating: "-", count: 60))")
    print(exampleName, terminator: "\n\n")
}
