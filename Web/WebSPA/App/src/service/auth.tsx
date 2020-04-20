import HttpService from "./_httpService";
import { stat } from "fs";

export function clearAuth() {
    sessionStorage.removeItem("tokenKey");
}

export function getID() {
    let item = sessionStorage.getItem("tokenKey");
    let id = 0;
    if (item) {
        id = JSON.parse(item).id;
    }
    return id;
}

export function getStudentID() {
    let item = sessionStorage.getItem("tokenKey");
    let id = 0;
    if (item) {
        id = JSON.parse(item).studentid;
    }
    return id;
}

export function isLogged() {
    let item = sessionStorage.getItem("tokenKey");
    if (item) {
        return true;
    } else {
        return false;
    }
}

export function getToken() {
    let item = sessionStorage.getItem("tokenKey");
    let token = null;
    if (item) {
        token = JSON.parse(item).access_token;
    }
    return token;
}

export function getLogin() {
    let item = sessionStorage.getItem("tokenKey");
    let login = null;
    if (item) {
        login = JSON.parse(item).login;
    }
    return login;
}

export function saveAuth(userid: number, studentid: number, token: string, login: string) {
    sessionStorage.setItem('tokenKey', JSON.stringify({ id: userid, login: login, studentid: studentid, access_token: token }));
}

export async function login(login: string, password: string) {

    let responseStatus: any;
    let result: any;
    if (login && password) {
        var dataBody = {
            login: login,
            password: password
        };
        const url = "api/Identity/authenticate"
        return await HttpService.post(url, dataBody, false, true)
            .then((data: any) => {
                const userId = data.userId === undefined ? 0 : data.userId
                if (userId == 0) {
                    result = { status: 400, message: data };
                } else {
                    result = { token: data.token, userid: data.userId, studentid: data.studentId, status: 200, login: data.login };
                }
                return result
            })
            .catch((ex) => {
                return ex;
            });
    }
}

export async function register(login: string, password: string) {

    let responseStatus: any;
    let result: any;
    if (login && password) {
        var dataBody = {
            login: login,
            password: password
        };
        const url = "api/Identity/register";
        result = await HttpService.post(url, dataBody, false, false)
            .then((data) => {
                if (data.status !== 200) {
                    result = { status: 400, message: data };
                } else {
                    result = data;
                }
                return result;
            })
            .catch((ex) => {
                return ex;
            });
        return result;
    }

}


export async function setStudent(userid: number, studentid: number) {
    let result: any;
    let responseStatus = 200;
    const dataBody = {
        UserId: userid,
        StudentId: studentid
    };
    const url = "api/Identity/setstudent";
    result = await HttpService.put(url, dataBody, true, false)
        .then((response: any) => {
            if (response.status !== 200) {
                responseStatus = response.status;
                return response.text();
            }
        })
        .then((data) => {
            if (responseStatus !== 200) {
                result = { status: responseStatus, context: data }
            } else {
                saveAuth(userid, studentid, getToken(), getLogin());
                result = { status: responseStatus }
            }
        })
        .catch((ex) => {
            return ex;
        });
    return result;
}

export async function getWebApi() {
    await fetch("/api/settings")
        .then((response: any) => {
            return response.json()
        })
        .then((data) => {
            sessionStorage.setItem('webApi', JSON.stringify({ webApi: data.webApi }));
            return data.webApi;
        })
        .catch((ex) => {
            return ex;
        });
}

async function PageSize() {
    const url = "api/Student/pageSize"
    await HttpService.get(url)
        .then((data: any) => {
            return data;
        }).catch((ex) => {
            return ex;
        });
}

export function getPageSize() {
    let item = sessionStorage.getItem("webApi");
    let url = null;
    if (item) {
        url = JSON.parse(item).pageSize;
    }
    return url;
}

export function getServer() {
    let item = sessionStorage.getItem("webApi");
    let url = null;
    if (item) {
        url = JSON.parse(item).webApi;
    }
    return url;
}
