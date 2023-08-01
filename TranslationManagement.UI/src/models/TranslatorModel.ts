import { TranslatorStatusEnum } from "../enums/TranslatorStatusEnum";

export interface TranslatorModel {
    id:number;
    name: string;
    hourlyRate: string;
    status: TranslatorStatusEnum;
    creditCardNumber: string;
}