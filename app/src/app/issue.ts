export interface Issue {
    id?: number;
    title: string;
    priority: number;
    creatorId: number;
    category?: string;
    categoryId: number;
    categoryColor?: string;
    description?: string;
    projectId: number;
}