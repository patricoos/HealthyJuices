export type ModelErrors = {
    [key: string]: string[]
};

export type ErrorsInfo = {
    message: string | undefined;
    modelErrors: ModelErrors | undefined;
    details: string | undefined;
}