import { JobStatusEnum } from "../enums/JobStatusEnum";

export interface TranslationJobModel {
    id: Number;
    customerName: string;
    status: JobStatusEnum;
    originalContent: string;
    translatedContent: string;
    price: Number;
}