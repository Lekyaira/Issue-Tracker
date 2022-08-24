export interface Issue {
    id?: number;
    title: string;
    priority: number;
    creator: string;
    creatorId: number;
    description?: string;
}