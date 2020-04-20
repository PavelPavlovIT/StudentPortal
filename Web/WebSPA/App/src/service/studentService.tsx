import { Subject } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { Student } from '../components/models/student';
import history from '../service/history';
import { getToken, getID, saveAuth } from './auth';
import { Page } from '../components/models/page';
import { Group } from '../components/models/group';
import HttpService from './_httpService';
import { PageGroup } from '../components/models/pageGroup';


const authorization = new BehaviorSubject(false)
export const authorizationService = {
    login: () => authorization.next(true),
    logout: () => authorization.next(false),
    getStatus: () => authorization.asObservable()
};


const messages = new BehaviorSubject("")

export const messageService = {
    sendMessage: (message: any) => messages.next(message),
    clearMessages: () => messages.next(""),
    getMessage: () => messages.asObservable()
};


export default class StudentService {
    static pageGroup = new BehaviorSubject<PageGroup>(new PageGroup())
    static page = new BehaviorSubject<Page>(new Page())

    static async loadAllGroups() {
        const url = "api/Student/listgroup"
        return await HttpService.get(url)
            .then((data: any) => {
                return data;
            }).catch((ex) => {
                return ex;
            });
    }

    static async loadGroups(pageIndex = 1) {
        const url = "api/Student/pagegroup?pageIndex=" + pageIndex
        return await HttpService.get(url)
            .then((data: any) => {
                const groups = data.records.map((item: any) => {
                    const group = new Group(item)
                    return group
                })
                const page = new PageGroup();
                page.currentPage = data.currentPage + 1;
                page.pageSize = data.pageSize;
                page.totalRecords = data.totalRecords;
                page.records = groups;
                this.pageGroup.next(page)
                return data;
            }).catch((ex) => {
                return ex;
            });
    }

    static async  loadStudent(pageIndex = 1, my: boolean, filter?: string, isMale?: boolean, orderBy?: string, desc?: boolean) {
        const url = "api/student/page"
            + "?pageIndex=" + pageIndex
            + "&id=" + (my ? getID() : 0)
            + "&filter=" + filter
            + (isMale == null ? "&isMale=" : "&isMale=" + isMale)
            + (orderBy == null ? "&orderBy=" : "&orderBy=" + orderBy)
            + (desc == null ? "&desc=" : "&desc=" + desc)
        return await HttpService.get(url)
            .then((data: any) => {
                if (data.status == 401)
                    return data;
                const students = data.records.map((item: any) => {
                    return new Student(item)
                })
                const page = new Page();
                page.currentPage = data.currentPage + 1;
                page.pageSize = data.pageSize;
                page.totalRecords = data.totalRecords;
                page.records = students;
                this.page.next(page)
                return data;
            }).catch((ex) => {
                return ex;
            });
    }
    static Students() {
        return this.page.asObservable();
    }

    static Groups() {
        return this.pageGroup.asObservable();
    }

    static async AddStudent(student: Student, groups: number[]) {
        let result: any;
        let responseStatus = 200;
        const dataBody = {
            firstName: student.firstName,
            lastName: student.lastName,
            patronymic: student.patronymic,
            isMale: student.isMale,
            nick: student.nick,
            userId: getID()
        };
        let url = "api/student/add";
        result = await HttpService.post(url, dataBody, true, true)
            .then((data: any) => {
                const studentId = data.id === undefined ? 0 : data.id
                if (studentId == 0) {
                    let index = data.indexOf('IX_Students_Nick');
                    if (index != -1) {
                        result = { status: data.status, context: "Nick is busy" }
                    } else {
                        result = { status: data.status, context: "Server returned error" }
                    }
                    return result;
                } else {
                    const student = new Student(data);
                    const page = this.page.getValue();
                    page.records.push(student)
                    page.totalRecords++;
                    const groupStudent = { StudentId: student.id, GroupIds: groups };
                    url = "api/Student/addGroupToStudent";
                    HttpService.put(url, groupStudent, true, false)
                        .then((data) => {
                        })
                        .catch((ex) => {
                        })

                    this.page.next(page);
                    history.push('/');
                    result = { status: 200, context: student };
                    return result;
                }
            })
            .catch((ex) => {
                return ex;
            });
        return result;
    }

    static async AddGroup(group: Group) {
        let result: any;
        const dataBody = {
            id: group.id,
            name: group.name,
        };
        const url = "api/student/addGroup";
        result = await HttpService.post(url, dataBody, true, true)
            .then((data: any) => {
                const groupId = data.id === undefined ? 0 : data.id
                if (groupId == 0) {
                    result = { status: data.status, context: "Server returned error" }
                } else {
                    const group = new Group(data);
                    const pageGroup = this.pageGroup.getValue();
                    pageGroup.records.push(group)
                    pageGroup.totalRecords++;
                    this.pageGroup.next(pageGroup);
                    result = { status: 200, context: group };
                    return result;
                }
            })
            .catch((ex) => {
                return ex;
            });
        return result;
    }

    static async UpdateStudent(student: Student, groups: number[]) {
        let result: any;
        student.groups = [];
        let url = "api/student/update"
        return await HttpService.put(url, student, true, false)
            .then((data) => {
                const status = data.status === undefined ? 400 : 200
                if (status == 400) {
                    let index = data.indexOf('IX_Students_Nick');
                    if (index != -1) {
                        result = { status: data.status, context: "Nick is busy" }
                    } else {
                        result = { status: data.status, context: "Server returned error" }
                    }
                    return result;
                } else {
                    result = { status: 200 }
                    const page = this.page.getValue();
                    page.records.forEach((t, i) => {
                        if (t.id === student.id) {
                            page.records[i] = student;
                        }
                    });
                    this.page.next(page);
                    const groupStudent = { StudentId: student.id, GroupIds: groups };
                    url = "api/Student/addGroupToStudent";
                    HttpService.put(url, groupStudent, true, false)
                        .then((data) => {
                        })
                        .catch((ex) => {
                        })
                    history.push('/');
                    return result;
                }
            })
            .catch((ex) => {
                return ex;
            });
    }

    static async UpdateGroup(group: Group) {
        let result: any;
        const url = "api/student/updateGroup"
        return await HttpService.put(url, group, true, false)
            .then((data) => {
                const page = this.pageGroup.getValue();
                page.records.forEach((t, i) => {
                    if (t.id === group.id) {
                        page.records[i] = group;
                    }
                });
                this.pageGroup.next(page);
                return result;
            })
            .catch((ex) => {
                return ex;
            });
    }
    static async RemoveStudent(studentId: number) {
        const url = "api/student/delete?studentId=" + studentId
        return await HttpService.delete(url, studentId, true)
            .then((data) => {
                const page = this.page.getValue();
                page.records.forEach((t, i) => {
                    if (t.id === studentId) {
                        page.records.splice(i, 1)
                    }
                });
                this.page.next(page);
                history.push('/');
            }).catch((ex) => {
                return ex;
            });
    }

    static async RemoveGroup(groupId: number) {
        const url = "api/student/deleteGroup?groupId=" + groupId
        return await HttpService.delete(url, groupId, true)
            .then((data) => {
                const pageGroup = this.pageGroup.getValue();
                pageGroup.records.forEach((t, i) => {
                    if (t.id === groupId) {
                        const test = pageGroup.records.splice(i, 1)
                    }
                });
                this.pageGroup.next(pageGroup);
                history.push('/group/all');
            }).catch((ex) => {
                return ex;
            });
    }
}

