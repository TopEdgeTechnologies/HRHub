// Interface
// Try creating a new interface and extending it like below
interface Rectangle {
    height: number,
    width: number
}

interface ColoredRectangle extends Rectangle {
    color: string
}

const coloredRectangle: ColoredRectangle = {
    height: 20,
    width: 10,
    color: "red"
};

console.log(coloredRectangle);
// End Interface

function printStatusCode(code: string | number) {
    console.log(`My status code is ${code}.`);
}

//The type of the value returned by the function can be explicitly defined.

// the `: number` here specifies that this function returns a number
function getTime(): number {
    return new Date().getTime();
}

//The type void can be used to indicate a function doesn't return any value.
function printHello(): void {
    console.log('Hello!');
}

//Function parameters are typed with a similar syntax as variable declarations.
function multiply(a: number, b: number) {
    return a * b;
}

//By default TypeScript will assume all parameters are required, but they can be explicitly marked as optional.
// the `?` operator here marks parameter `c` as optional
function add(a: number, b: number, c?: number) {
    return a + b + (c || 0);
}

//For parameters with default values, the default value goes after the type annotation:
/*
function pow(value: number, exponent: number = 10) {
    return value ** exponent;
}
*/

//Typing named parameters follows the same pattern as typing normal parameters.
function divide({ dividend, divisor }: { dividend: number, divisor: number }) {
    return dividend / divisor;
}

//Rest parameters can be typed like normal parameters, but the type must be an array as rest parameters are always arrays.
/*
function add(a: number, b: number, ...rest: number[]) {
    return a + b + rest.reduce((p, c) => p + c, 0);
}
*/

/*
Type Alias
Function types can be specified separately from functions with type aliases.

These types are written similarly to arrow functions, read more about arrow functions here.
*/

type Negate = (value: number) => number;

// in this function, the parameter `value` automatically gets assigned the type `number` from the type `Negate`
const negateFunction: Negate = (value) => value * -1;


class Person {
    name: string;
}

const person = new Person();
person.name = "Jane";

console.log(person);
// Output: Person { name: 'Jane' }

class Person {
    private name: string;

    public constructor(name: string) {
        this.name = name;
    }

    public getName(): string {
        return this.name;
    }
}

const person = new Person("Muhammad");

console.log(person.getName()); // person.name isn't accessible from outside the class since it's private


// Define a click event handler for the button
document.getElementById("btnShowName").addEventListener("click", function () {
    // Get the values from the textboxes
    const firstName = (<HTMLInputElement>document.getElementById("txtFirstName")).value;
    const lastName = (<HTMLInputElement>document.getElementById("txtLastName")).value;

    // Display the names
    document.getElementById("txt_fname").innerText = firstName;
    document.getElementById("txt_lastName").innerText = lastName;
});


