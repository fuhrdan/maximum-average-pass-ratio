//*****************************************************************************
//** 1792. Maximum Average Pass Ratio                               leetcode **
//*****************************************************************************
typedef struct {
    int pass;
    int total;
    double gain;
} Class;

// Helper function to calculate the gain of adding one student
double calculateGain(int pass, int total) {
    return (double)(pass + 1) / (total + 1) - (double)pass / total;
}

// Swap two elements in the heap
void swap(Class* a, Class* b) {
    Class temp = *a;
    *a = *b;
    *b = temp;
}

// Heapify down for max-heap
void heapifyDown(Class* heap, int size, int index) {
    int largest = index;
    int left = 2 * index + 1;
    int right = 2 * index + 2;

    if (left < size && heap[left].gain > heap[largest].gain) {
        largest = left;
    }
    if (right < size && heap[right].gain > heap[largest].gain) {
        largest = right;
    }
    if (largest != index) {
        swap(&heap[index], &heap[largest]);
        heapifyDown(heap, size, largest);
    }
}

// Heapify up for max-heap
void heapifyUp(Class* heap, int index) {
    int parent = (index - 1) / 2;
    if (index > 0 && heap[index].gain > heap[parent].gain) {
        swap(&heap[index], &heap[parent]);
        heapifyUp(heap, parent);
    }
}

double maxAverageRatio(int** classes, int classesSize, int* classesColSize, int extraStudents) {
    // Create a max-heap for classes based on gain
    Class* heap = (Class*)malloc(classesSize * sizeof(Class));
    int heapSize = 0;

    for (int i = 0; i < classesSize; i++) {
        int pass = classes[i][0];
        int total = classes[i][1];
        double gain = calculateGain(pass, total);
        heap[heapSize++] = (Class){pass, total, gain};
        heapifyUp(heap, heapSize - 1);
    }

    // Assign extra students to maximize the gain
    while (extraStudents-- > 0) {
        // Extract the class with the highest gain
        Class top = heap[0];
        top.pass++;
        top.total++;
        top.gain = calculateGain(top.pass, top.total);
        heap[0] = top;
        heapifyDown(heap, heapSize, 0);
    }

    // Calculate the final average pass ratio
    double totalPassRatio = 0.0;
    for (int i = 0; i < classesSize; i++) {
        totalPassRatio += (double)heap[i].pass / heap[i].total;
    }

    free(heap);
    return totalPassRatio / classesSize;
}