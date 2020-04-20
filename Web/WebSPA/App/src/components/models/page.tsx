import { Student } from "./student";

export class Page {
    currentPage: number = 0;
    pageSize: number = 0;
    totalRecords: number = 0;
    records: Student[] = [];
}