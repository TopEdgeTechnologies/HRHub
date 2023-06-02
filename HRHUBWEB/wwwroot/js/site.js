function numberToWords(amount) {
    var singleDigits = [
        "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
        "seventeen", "eighteen", "nineteen"
    ];

    var tensDigits = [
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    ];

    var scales = ["", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion"];

    if (amount === 0) {
        return "zero";
    }

    var words = [];

    function convertThreeDigit(number) {
        var word = "";
        var hundreds = Math.floor(number / 100);
        var tens = Math.floor((number % 100) / 10);
        var units = number % 10;

        if (hundreds !== 0) {
            word += singleDigits[hundreds] + " hundred ";
        }

        if (tens >= 2) {
            word += tensDigits[tens] + " ";
            word += singleDigits[units];
        } else if (tens === 1) {
            word += singleDigits[tens * 10 + units];
        } else if (units !== 0) {
            word += singleDigits[units];
        }

        return word.trim();
    }

    var scaleIndex = 0;

    while (amount > 0) {
        var threeDigit = amount % 1000;
        if (threeDigit !== 0) {
            var scaleWord = scales[scaleIndex];
            var digitWord = convertThreeDigit(threeDigit);
            var formattedWord = digitWord + " " + scaleWord;
            words.unshift(formattedWord.trim());
        }
        amount = Math.floor(amount / 1000);
        scaleIndex++;
    }

    return words.join(" ");
}
