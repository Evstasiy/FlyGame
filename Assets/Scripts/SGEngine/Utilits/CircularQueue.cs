using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.SGEngine.Utilits
{
    public class CircularQueue<T> : IEnumerable<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int size;
        private bool isClosed;
        private int currentIndex;  // Для перемещения вперед/назад

        public CircularQueue(int initialCapacity = 3)
        {
            buffer = new T[initialCapacity];
            head = 0;
            tail = 0;
            size = 0;
            isClosed = false;
            currentIndex = -1;  // Изначально нет текущей позиции
        }

        public bool IsFull => size == buffer.Length;
        public bool IsEmpty => size == 0;

        public void Enqueue(T item)
        {
            if (isClosed)
            {
                throw new InvalidOperationException("Queue is closed.");
            }

            if (IsFull)
            {
                ResizeBuffer();
            }

            buffer[tail] = item;
            tail = (tail + 1) % buffer.Length;
            size++;

            if (currentIndex == -1)
            {
                currentIndex = head;  // Устанавливаем текущий элемент при первой вставке
            }
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            T item = buffer[head];
            head = (head + 1) % buffer.Length;
            size--;
            if (size == 0)
            {
                currentIndex = -1;  // Очередь пуста, сбрасываем позицию
            }

            return item;
        }

        public void Close()
        {
            isClosed = true;
        }

        private void ResizeBuffer()
        {
            int newCapacity = buffer.Length * 2;
            T[] newBuffer = new T[newCapacity];

            for (int i = 0; i < size; i++)
            {
                newBuffer[i] = buffer[(head + i) % buffer.Length];
            }

            buffer = newBuffer;
            head = 0;
            tail = size;
        }

        // Реализация IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return buffer[(head + i) % buffer.Length];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Перемещение вперед
        public T GetNext()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            if (currentIndex == -1)
            {
                currentIndex = head;  // Устанавливаем начальный элемент, если еще не установлен
            }

            return buffer[(currentIndex + 1) % buffer.Length];
        }
        
        // Перемещение назад
        public T GetPrevious()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            if (currentIndex == -1)
            {
                currentIndex = head;  // Устанавливаем начальный элемент, если еще не установлен
            }
            return buffer[(currentIndex - 1 + buffer.Length) % buffer.Length];
        }

        public T MoveNext()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            if (currentIndex == -1)
            {
                currentIndex = head;  // Устанавливаем начальный элемент, если еще не установлен
            }

            currentIndex = (currentIndex + 1) % buffer.Length;  // Циклический сдвиг вперед
            return buffer[currentIndex];
        }

        // Перемещение назад
        public T MovePrevious()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            if (currentIndex == -1)
            {
                currentIndex = head;  // Устанавливаем начальный элемент, если еще не установлен
            }

            currentIndex = (currentIndex - 1 + buffer.Length) % buffer.Length;  // Циклический сдвиг назад
            return buffer[currentIndex];
        }

        /// <summary>
        /// Получить текущий элемент
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Current()
        {
            if (IsEmpty || currentIndex == -1)
            {
                throw new InvalidOperationException("Queue is empty or no current item.");
            }

            return buffer[currentIndex];
        }
    }
}
