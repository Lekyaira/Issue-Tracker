export interface Issue {
    id?: number;
    title: string;
    priority: number;
    creatorId: string;
    category?: string;
    categoryId: number;
    categoryColor?: string;
    description?: string;
}