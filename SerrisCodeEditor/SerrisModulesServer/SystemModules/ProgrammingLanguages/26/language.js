return {
    // escape codes
    control: /[\\`*_\[\]{}()#+\-\.!]/,
    noncontrol: /[^\\`*_\[\]{}()#+\-\.!]/,
    escapes: /\\(?:@control)/,

    // escape codes for javascript/CSS strings
    jsescapes: /\\(?:[btnfr\\"']|[0-7][0-7]?|[0-3][0-7]{2})/,

    // non matched elements
    empty: [
        'area', 'base', 'basefont', 'br', 'col', 'frame',
        'hr', 'img', 'input', 'isindex', 'link', 'meta', 'param'
    ],

    tokenizer: {
        root: [
            // headers
            [/^(\s*)(#+)((?:[^\\#]|@escapes)+)((?:#+)?)/, ['white', 'keyword.$1', 'keyword.$1', 'keyword.$1']],
            [/^\s*(=+|\-+)\s*$/, 'keyword.header'],
            [/^\s*((\*[ ]?)+)\s*$/, 'keyword.header'],

            // code & quote
            [/^\s*>+/, 'string.quote'],
            [/^(\t|[ ]{4}).*$/, 'namespace.code'], // code line
            [/^\s*~+\s*$/, { token: 'namespace.code', bracket: '@open', next: '@codeblock' }],

            // github style code blocks
            [/^\s*````\s*(\w+)\s*$/, { token: 'namespace.code', bracket: '@open', next: '@codeblockgh', nextEmbedded: 'text/x-$1' }],
            [/^\s*````\s*((?:\w|[\/\-])+)\s*$/, { token: 'namespace.code', bracket: '@open', next: '@codeblockgh', nextEmbedded: '$1' }],

            // list
            [/^\s*([\*\-+:]|\d\.)/, 'string.list'],

            // markup within lines
            { include: '@linecontent' },
        ],

        codeblock: [
            [/^\s*~+\s*$/, { token: 'namespace.code', bracket: '@close', next: '@pop' }],
            [/.*$/, 'namespace.code'],
        ],

        // github style code blocks
        codeblockgh: [
            [/````\s*$/, { token: '@rematch', bracket: '@close', switchTo: '@codeblockghend', nextEmbedded: '@pop' }],
            [/[^`]*$/, 'namespace.code'],
        ],

        codeblockghend: [
            [/\s*````/, { token: 'namespace.code', bracket: '@close', next: '@pop' }],
            [/./, '@rematch', '@pop'],
        ],

        linecontent: [
            // [/\s(?=<(\w+)[^>]*>)/, {token: 'html', next: 'html.$1', nextEmbedded: 'text/html' } ],
            // [/<(\w+)[^>]*>/, {token: '@rematch', next: 'html.$1', nextEmbedded: 'text/html' } ],

            // escapes
            [/&\w+;/, 'string.escape'],
            [/@escapes/, 'escape'],

            // various markup
            [/\b__([^\\_]|@escapes|_(?!_))+__\b/, 'strong'],
            [/\*\*([^\\*]|@escapes|\*(?!\*))+\*\*/, 'strong'],
            [/\b_[^_]+_\b/, 'emphasis'],
            [/\*([^\\*]|@escapes)+\*/, 'emphasis'],
            [/`([^\\`]|@escapes)+`/, 'namespace.code'],

            // links
            [/\{[^}]+\}/, 'string.target'],
            [/(!?\[)((?:[^\]\\]|@escapes)+)(\]\([^\)]+\))/, ['string.link', '', 'string.link']],
            [/(!?\[)((?:[^\]\\]|@escapes)+)(\])/, 'string.link'],

            // or html
            { include: 'html' },
        ],

        html: [
            // html tags
            [/<(\w+)\/>/, 'tag.tag-$1'],
            [/<(\w+)/, {
                cases: {
                    '@empty': { token: 'tag.tag-$1', next: '@tag.$1' },
                    '@default': { token: 'tag.tag-$1', bracket: '@open', next: '@tag.$1' }
                }
            }],
            [/<\/(\w+)\s*>/, { token: 'tag.tag-$1', bracket: '@close', next: '@pop' }],

            // whitespace
            { include: '@whitespace' },
        ],


        // whitespace and (html style) comments
        whitespace: [
            [/[ ]{2}$/, 'invalid'],
            [/[ \t\r\n]+/, 'white'],
            [/<!--/, 'comment', '@comment']
        ],

        comment: [
            [/[^<\-]+/, 'comment.content'],
            [/-->/, 'comment', '@pop'],
            [/<!--/, 'comment.content.invalid'],
            [/[<\-]/, 'comment.content']
        ],

        // Almost full HTML tag matching, complete with embedded scripts & styles
        tag: [
            [/[ \t\r\n]+/, 'white'],
            [/(type)(\s*=\s*)(")([^"]+)(")/, ['attribute.name', 'delimiter', 'attribute.value',
                { token: 'attribute.value', switchTo: '@tag.$S2.$4' },
                'attribute.value']],
            [/(type)(\s*=\s*)(')([^']+)(')/, ['attribute.name', 'delimiter', 'attribute.value',
                { token: 'attribute.value', switchTo: '@tag.$S2.$4' },
                'attribute.value']],
            [/(\w+)(\s*=\s*)("[^"]*"|'[^']*')/, ['attribute.name', 'delimiter', 'attribute.value']],
            [/\w+/, 'attribute.name'],
            [/\/>/, 'tag.tag-$S2', '@pop'],
            [/>/, {
                cases: {
                    '$S2==style': { token: 'tag.tag-$S2', switchTo: '@embedded.$S2', nextEmbedded: 'text/css' },
                    '$S2==script': {
                        cases: {
                            '$S3': { token: 'tag.tag-$S2', switchTo: '@embedded.$S2', nextEmbedded: '$S3' },
                            '@default': { token: 'tag.tag-$S2', switchTo: '@embedded.$S2', nextEmbedded: 'mjavascript' }
                        }
                    },
                    '@default': { token: 'tag.tag-$S2', switchTo: 'html' }
                }
            }],
        ],

        embedded: [
            [/[^"'<]+/, ''],
            [/<\/(\w+)\s*>/, {
                cases: {
                    '$1==$S2': { token: '@rematch', switchTo: '@html', nextEmbedded: '@pop' },
                    '@default': ''
                }
            }],
            [/"([^"\\]|\\.)*$/, 'string.invalid'],  // non-teminated string
            [/'([^'\\]|\\.)*$/, 'string.invalid'],  // non-teminated string
            [/"/, 'string', '@string."'],
            [/'/, 'string', '@string.\''],
            [/</, '']
        ],

        // scan embedded strings in javascript or css
        string: [
            [/[^\\"']+/, 'string'],
            [/@jsescapes/, 'string.escape'],
            [/\\./, 'string.escape.invalid'],
            [/["']/, {
                cases: {
                    '$#==$S2': { token: 'string', next: '@pop' },
                    '@default': 'string'
                }
            }]
        ],

    }
}