export interface Issue {
    id?: number;
    title: string;
    priority: number;
    creator?: string;
    creatorId: number;
    category?: string;
    categoryId: number;
    categoryColor?: string;
    description?: string;
}