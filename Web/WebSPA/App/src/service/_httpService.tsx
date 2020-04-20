import { dispatch } from "rxjs/internal/observable/pairs";
import { getToken, getServer } from "./auth";
import history from '../service/history';

const server = getServer();
export default class HttpService {

    static async  get(url: string) {
        let header: any;
        header = { 'Content-Type': 'application/json', 'Authorization': getToken() }
        const result = await fetch(server + url,
            {
                method: 'GET',
                headers: header,
            })
            .then((response) => {
                if (response.status == 401) {
                    return response
                } else {
                    return response.json()
                }
            })
        return result;
    }

    static async  post(url: string, body: any, auth: boolean, json: boolean) {
        let responseStatus: any;
        let header: any;
        header = auth ? { 'Content-Type': 'application/json', 'Authorization': getToken() } : { 'Content-Type': 'application/json' }
        const result = await fetch(server + url,
            {
                method: 'POST',
                headers: header,
                body: JSON.stringify(body)
            })
            .then((response: any) => {
                if (response.status !== 200) {
                    return response.text()
                } else {
                    responseStatus = response.status;
                    return json ? response.json() : response
                }
            })
        return result;
    }

    static async put(url: string, body: any, auth: boolean, json: boolean) {
        let header: any;
        header = auth ? { 'Content-Type': 'application/json', 'Authorization': getToken() } : { 'Content-Type': 'application/json' }
        const result = await fetch(server + url,
            {
                method: 'PUT',
                headers: header,
                body: JSON.stringify(body)
            })
            .then((response: any) => {
                if (response.status !== 200) {
                    return response.text();
                } else {
                    const result = { status: response.status, response: json ? response.json() : response }
                    return result;
                }
            })
        return result;
    }

    static async delete(url: string, studentId: number, auth: boolean) {
        let header: any = auth ? { 'Content-Type': 'application/json', 'Authorization': getToken() } : { 'Content-Type': 'application/json' }

        const result = await fetch(server + url,
            {
                method: 'DELETE',
                headers: header,
            })
            .then((response) => {
                return response;
            })
        return result;
    }
}