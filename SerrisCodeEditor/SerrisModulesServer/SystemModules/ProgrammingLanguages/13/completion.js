﻿var brackets = { "surroundingPairs": [{ "open": "{", "close": "}" }, { "open": "(", "close": ")" }, { "open": "[", "close": "]" }], "autoClosingPairs": [{ "open": "{", "close": "}" }, { "open": "(", "close": ")" }, { "open": "[", "close": "]" }, { "open": "'", "close": "'" }, { "open": '"', "close": '"' }], "brackets": [["{", "}"], ["(", ")"], ["[", "]"]] };
monaco.languages.setLanguageConfiguration("python", brackets);