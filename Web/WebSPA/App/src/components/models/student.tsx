import { Group } from "./group";

export class Student {
    id: number = 0;
    isMale: boolean = true;
    firstName: string = "";
    lastName: string = "";
    patronymic: string = "";
    nick: string = "";
    groups: Group[] = [];

    constructor(val: any) {
        if (typeof val !== 'undefined') {
            this.id = (typeof val.id === 'undefined') ? 0 : val.id;
            this.isMale = (typeof val.gender === 'undefined') ? val.isMale : ((val.gender === 'male') ? true : false);
            this.firstName = val.firstName;
            this.lastName = val.lastName;
            this.patronymic = val.patronymic;
            this.nick = (typeof val.nick === 'undefined' || val.nick === '') ? null : val.nick;
            this.groups = [];
            if (val.groups !== undefined)
                this.groups = val.groups.map((item: any) => {
                    return new Group(item)
                })
        }
    }

}