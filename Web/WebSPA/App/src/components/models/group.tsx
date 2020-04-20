export class Group {
    id: number = 0;
    name: string = "";
    constructor(val: any) {
        if (val !== undefined) {
            this.id = val.id == undefined ? 0 : val.id;
            this.name = val.name == undefined ? "" : val.name
        }
    }
}