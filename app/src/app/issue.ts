export interface Issue {
    id: number;
    title: string;
    priority: number;
    creator: string;
    description?: string;
}